﻿using Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

[DefaultExecutionOrder(-10)]
public class ShotCore : MonoBehaviour
{
    [SerializeField] [Min(0)] private float lingerTime = 2;
    [SerializeField] Gradient colorOverTime;
    private float lingerTimeRemaining = 0;

    [SerializeField] private LineRenderer trailPrefab;
    private List<LineRenderer> trails = new List<LineRenderer>();

    [NonSerialized] public ICombatAffector source;
    [NonSerialized] public ICombatTarget target;
    [InspectorReadOnly] public bool isEmpowered = false;

    [Serializable]
    public class PathSeg //Struct is more performant, but also leads to awful syntax
    {
        public RaycastHit hit;
        public Vector3 start;
        public Vector3 end => hit.point;
        public Vector3 direction => (end-start).normalized;
    }
    [NonSerialized] public List<PathSeg> pathSegments;
    public PathSeg lastSeg => pathSegments[pathSegments.Count-1];

    private void Start()
    {
        pathSegments = new List<PathSeg>();

        AppendPathSegment(new Ray(transform.position, transform.forward), true);

        //Setup trail
        lingerTimeRemaining = lingerTime;
        foreach (PathSeg seg in pathSegments)
        {
            LineRenderer trail = Instantiate(trailPrefab, transform);
            trails.Add(trail);
            trail.SetPosition(0, seg.start);
            trail.SetPosition(1, seg.end);
        }

        //Apply all effects
        foreach (ShotEffect e in GetComponents<ShotEffect>()) e.Hit(source, target, lastSeg.hit);
    }

    public void AppendPathSegment(Ray ray, bool useShotResolvers)
    {
        //Default case: Raycast
        PathSeg raycast = new PathSeg() { start = ray.origin };
        if (Physics.Raycast(ray, out raycast.hit)) target = raycast.hit.collider.GetComponentInParent<ICombatTarget>();
        else raycast.hit.point = ray.origin + ray.direction * 100; //Ensure good hit location, even if firing at the air
        pathSegments.Add(raycast);

        if (useShotResolvers)
        {
            //Apply path resolvers
            //Fallback target selectors like Magnetism
            //Special interactions like the Coin or Portal
            foreach (ShotPathResolver fallback in GetComponents<ShotPathResolver>()) fallback.ResolvePath();
        }
    }

    private void Update()
    {
        lingerTimeRemaining -= Time.deltaTime;
        if (lingerTimeRemaining < 0) Destroy(gameObject);

        foreach (LineRenderer trail in trails) trail.startColor = trail.endColor = colorOverTime.Evaluate(1 - lingerTimeRemaining/lingerTime);
    }
}