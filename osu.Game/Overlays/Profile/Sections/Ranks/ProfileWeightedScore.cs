﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Scoring;
using osuTK;

namespace osu.Game.Overlays.Profile.Sections.Ranks
{
    public class ProfileWeightedScore : ProfileScore
    {
        private readonly double weight;

        public ProfileWeightedScore(ScoreInfo score, double weight)
            : base(score)
        {
            this.weight = weight;
        }

        protected override Drawable CreateRightContent() => new FillFlowContainer
        {
            AutoSizeAxes = Axes.Both,
            Direction = FillDirection.Vertical,
            Spacing = new Vector2(0, 2),
            Children = new Drawable[]
            {
                new FillFlowContainer
                {
                    AutoSizeAxes = Axes.Both,
                    Direction = FillDirection.Horizontal,
                    Spacing = new Vector2(15, 0),
                    Children = new Drawable[]
                    {
                        CreateDrawableAccuracy(),
                        new FillFlowContainer
                        {
                            AutoSizeAxes = Axes.Both,
                            Direction = FillDirection.Horizontal,
                            Children = new[]
                            {
                                new OsuSpriteText
                                {
                                    Anchor = Anchor.BottomLeft,
                                    Origin = Anchor.BottomLeft,
                                    Font = OsuFont.GetFont(weight: FontWeight.Bold, italics: true),
                                    Text = $"{Score.PP * weight:0}",
                                },
                                new OsuSpriteText
                                {
                                    Anchor = Anchor.BottomLeft,
                                    Origin = Anchor.BottomLeft,
                                    Font = OsuFont.GetFont(size: 14, weight: FontWeight.Bold, italics: true),
                                    Text = "pp",
                                }
                            }
                        }
                    }
                },
                new OsuSpriteText
                {
                    Font = OsuFont.GetFont(size: 12, weight: FontWeight.Bold),
                    Text = $@"weighted {weight:P0}"
                }
            }
        };
    }
}
